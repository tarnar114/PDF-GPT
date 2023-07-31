import { Card, CardBody, CardHeader, Text, Heading ,Tag} from "@chakra-ui/react";
import React from "react";

export default function Prompt({role,text}) {
  return (
    <Card w="100%" minH="12%" borderRadius="24px">
      <CardBody padding='12px' m={4}>

        <Tag borderRadius="full" marginBottom={2}>{role}</Tag>
        <Text>
          {text}

        </Text>
      </CardBody>
    </Card>
  );
}
