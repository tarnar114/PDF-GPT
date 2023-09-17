import {
  Card,
  Stack,
  CardBody,
  Heading,
  Text,
  CardFooter,
  Button,
  Icon,
} from "@chakra-ui/react";
import React from "react";
import { AiFillFileText } from "react-icons/ai";
export default function FileCard({fileName}) {
  return (
    <Card
      variant="filled"
      direction="row"
      align="center"
      w="90%"
    >
      <Icon as={AiFillFileText} boxSize="2em" ml="10%" />
      <Stack>
        <CardBody align="center" overflow="hidden" >
          <Text noOfLines="1">{fileName}</Text>
        </CardBody>
      </Stack>
    </Card>
  );
}
